const chromeLauncher = require('chrome-launcher');
const { execSync } = require('child_process');
const fs = require('fs');
(async () => {
  const chrome = await chromeLauncher.launch({chromeFlags: ['--headless', '--no-sandbox']});
  try {
    const cmd = `npx lighthouse http://localhost:5044 --only-categories=accessibility --output=json --output-path=lighthouse-report.json --port=${chrome.port} --quiet`;
    console.log('Running:', cmd);
    execSync(cmd, { stdio: 'inherit' });
    console.log('Lighthouse report saved to lighthouse-report.json');
  } finally {
    await chrome.kill();
  }
})();
