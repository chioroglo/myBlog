self.addEventListener('install', (event) => {
    console.log('[Service Worker] Installed');
  });
  
  self.addEventListener('activate', (event) => {
    console.log('[Service Worker] Activated');
  });