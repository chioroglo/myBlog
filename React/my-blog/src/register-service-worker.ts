export function registerServerWorker(): void {
    if ('serviceWorker' in navigator) {
      window.addEventListener('load', () => {
        navigator.serviceWorker
          .register('/service-worker.js')
          .then((registration: ServiceWorkerRegistration) => {
            console.log('Service Worker registered:', registration);
          })
          .catch((error: Error) => {
            console.error('Service Worker registration failed:', error);
          });
      });
    }
  }