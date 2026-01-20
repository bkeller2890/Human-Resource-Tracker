const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch({ args: ['--no-sandbox'] });
  const page = await browser.newPage({ viewport: { width: 1200, height: 900 } });
  await page.goto('http://localhost:5044/', { waitUntil: 'networkidle' });
  // Wait a bit for Chart.js to render
  await page.waitForTimeout(800);
  await page.screenshot({ path: 'dashboard.png', fullPage: true });
  await browser.close();
  console.log('Screenshot saved to dashboard.png');
})();
