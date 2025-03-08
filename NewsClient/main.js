const { app, BrowserWindow } = require('electron');
const path = require('path');

let mainWindow;

function createWindow() {
  mainWindow = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      nodeIntegration: false, // Отключаем Node.js в рендере для безопасности
      contextIsolation: true, // Включаем изоляцию контекста
      preload: path.join(__dirname, 'preload.js') // Подключаем preload-скрипт
    }
  });

  // Загружаем Angular-приложение
  mainWindow.loadURL(
    process.env.NODE_ENV === 'development'
      ? 'http://localhost:4200' // Для разработки
      : `file://${path.join(__dirname, 'dist/news-client/browser/index.html')}` // Для production
  );

  // Открываем DevTools (опционально)
  if (process.env.NODE_ENV === 'development') {
    mainWindow.webContents.openDevTools();
  }

  mainWindow.on('closed', () => {
    mainWindow = null;
  });
}

app.whenReady().then(() => {
  createWindow();

  app.on('activate', () => {
    if (BrowserWindow.getAllWindows().length === 0) {
      createWindow();
    }
  });
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});
