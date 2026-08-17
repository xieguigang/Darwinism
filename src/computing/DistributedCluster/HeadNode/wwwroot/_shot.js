const { chromium } = require('playwright');
(async () => {
  const browser = await chromium.launch();
  const page = await browser.newPage({ viewport: { width: 1440, height: 900 } });
  await page.goto('http://localhost:8080/', { waitUntil: 'networkidle' });
  await page.waitForTimeout(800);
  // 点击提交任务按钮
  await page.click('#btnOpenSubmit');
  await page.waitForTimeout(600);
  const modal = await page.$('#submitModal .modal');
  const box = await modal.boundingBox();
  console.log('modal size:', JSON.stringify(box));
  await page.screenshot({ path: 'submit_modal.png', fullPage: false });
  console.log('screenshot saved');
  await browser.close();
})();
