const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');
(async () => {
  const browser = await chromium.launch({ args: ['--no-sandbox'] });
  const page = await browser.newPage({ viewport: { width: 1200, height: 900 } });
  await page.goto('http://localhost:5044/', { waitUntil: 'networkidle' });
  // inject axe
  const axeSource = require('axe-core').source;
  await page.addScriptTag({ content: axeSource });
  // run axe
  const results = await page.evaluate(async () => {
    return await new Promise(resolve => {
      axe.run({ runOnly: { type: 'tag', values: ['wcag2a', 'wcag2aa'] } }, (err, results) => {
        if (err) resolve({ error: err.toString() });
        else resolve(results);
      });
    });
  });
  const outPath = path.resolve('axe-report.json');
  fs.writeFileSync(outPath, JSON.stringify(results, null, 2));
  console.log('Axe report saved to', outPath);
  await browser.close();
})();
