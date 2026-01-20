// Dashboard charts script extracted from _DashboardCharts.cshtml
const ChartOptionsBase = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
        legend: { position: 'bottom' },
        tooltip: {
            callbacks: {
                label: function(context) {
                    const label = context.label || '';
                    const value = context.formattedValue || context.raw || 0;
                    return `${label}: ${value}`;
                }
            }
        }
    }
};

function colorPalette(n) {
    const palette = ['#4e79a7','#f28e2b','#e15759','#76b7b2','#59a14f','#edc949','#af7aa1','#ff9da7','#9c755f','#bab0ac'];
    const colors = [];
    for (let i = 0; i < n; i++) colors.push(palette[i % palette.length]);
    return colors;
}

async function fetchJson(url) {
    const resp = await fetch(url, { headers: { 'Accept': 'application/json' } });
    if (!resp.ok) throw new Error('Network error');
    return resp.json();
}

(async function renderDashboardCharts() {
    try {
        const leaveData = await fetchJson('/Home/Api/LeaveStatusCounts');
        const leaveLabels = leaveData.map(x => x.status || x.Status);
        const leaveCounts = leaveData.map(x => x.count || x.Count);
        const leaveColors = colorPalette(leaveLabels.length);

        const leaveDesc = document.getElementById('leaveChartDesc');
        if (leaveDesc) {
            let html = 'Status counts: ' + leaveLabels.map((l, i) => `${l} ${leaveCounts[i]}`).join(', ');
            leaveDesc.textContent = html;
        }
        try {
            const leaveTableBody = document.getElementById('leaveDataNoScript');
            if (leaveTableBody) {
                leaveTableBody.innerHTML = leaveLabels.map((l,i) => `<tr><td>${l}</td><td>${leaveCounts[i]}</td></tr>`).join('');
            }
        } catch (e) { }

        const leaveCtx = document.getElementById('leaveStatusChart').getContext('2d');
        window.leaveChart = new Chart(leaveCtx, {
            type: 'doughnut',
            data: { labels: leaveLabels, datasets: [{ data: leaveCounts, backgroundColor: leaveColors, hoverOffset: 6 }] },
            options: Object.assign({}, ChartOptionsBase)
        });
        window.leaveCurrentIndex = 0;

        const empData = await fetchJson('/Home/Api/EmployeeCountsByDepartment');
        const empLabels = empData.map(x => x.department || x.Department);
        const empCounts = empData.map(x => x.count || x.Count);
        const empColors = colorPalette(empLabels.length);

        const empDesc = document.getElementById('empChartDesc');
        if (empDesc) {
            let html = 'Employee counts by department: ' + empLabels.map((l,i) => `${l} ${empCounts[i]}`).join(', ');
            empDesc.textContent = html;
        }
        try {
            const empTableBody = document.getElementById('empDataNoScript');
            if (empTableBody) empTableBody.innerHTML = empLabels.map((l,i) => `<tr><td>${l}</td><td>${empCounts[i]}</td></tr>`).join('');
        } catch (e) { }

        const empCtx = document.getElementById('employeeDeptChart').getContext('2d');
        window.empChart = new Chart(empCtx, {
            type: 'bar',
            data: { labels: empLabels, datasets: [{ label: 'Employees', data: empCounts, backgroundColor: empColors }] },
            options: Object.assign({}, ChartOptionsBase, { scales: { y: { beginAtZero: true } } })
        });
        window.empCurrentIndex = 0;

        let live = document.getElementById('dashboardUpdateLive');
        if (!live) {
            live = document.createElement('div');
            live.id = 'dashboardUpdateLive';
            live.setAttribute('aria-live','polite');
            live.className = 'visually-hidden';
            document.body.appendChild(live);
        }
        live.textContent = `Dashboard updated: ${leaveLabels.length} leave statuses; ${empLabels.length} departments.`;

        const wrappers = document.querySelectorAll('.chart-wrapper');
        function announce(text) { if (!live) return; live.textContent = text; }

        function announceDataPoint(chart, index) {
            if (!chart) return;
            const labels = chart.data.labels || [];
            const dataset = (chart.data.datasets && chart.data.datasets[0]) || {};
            const value = (dataset.data && dataset.data[index]) || 0;
            const label = labels[index] || `item ${index+1}`;
            announce(`${label}: ${value}`);
        }

        wrappers.forEach(w => {
            const descId = w.getAttribute('aria-describedby');
            const isLeave = !!w.querySelector('#leaveStatusChart');
            const isEmp = !!w.querySelector('#employeeDeptChart');

            w.addEventListener('focus', () => {
                const descEl = descId ? document.getElementById(descId) : null;
                if (descEl) announce(descEl.textContent);
                if (isLeave) announceDataPoint(window.leaveChart, window.leaveCurrentIndex || 0);
                if (isEmp) announceDataPoint(window.empChart, window.empCurrentIndex || 0);
            });

            w.addEventListener('keydown', (ev) => {
                const key = ev.key;
                if (['ArrowLeft','ArrowUp','ArrowRight','ArrowDown'].includes(key)) {
                    ev.preventDefault();
                    if (isLeave && window.leaveChart) {
                        const max = (window.leaveChart.data.labels || []).length - 1;
                        if (key === 'ArrowLeft' || key === 'ArrowUp') window.leaveCurrentIndex = Math.max(0, (window.leaveCurrentIndex||0)-1);
                        else window.leaveCurrentIndex = Math.min(max, (window.leaveCurrentIndex||0)+1);
                        announceDataPoint(window.leaveChart, window.leaveCurrentIndex);
                    }
                    if (isEmp && window.empChart) {
                        const max = (window.empChart.data.labels || []).length - 1;
                        if (key === 'ArrowLeft' || key === 'ArrowUp') window.empCurrentIndex = Math.max(0, (window.empCurrentIndex||0)-1);
                        else window.empCurrentIndex = Math.min(max, (window.empCurrentIndex||0)+1);
                        announceDataPoint(window.empChart, window.empCurrentIndex);
                    }
                }
                if (ev.key === 'Enter' || ev.key === ' ') {
                    ev.preventDefault();
                    if (isLeave) announceDataPoint(window.leaveChart, window.leaveCurrentIndex||0);
                    if (isEmp) announceDataPoint(window.empChart, window.empCurrentIndex||0);
                }
            });
        });

    } catch (err) { console.error('Dashboard charts failed to render', err); }
})();
