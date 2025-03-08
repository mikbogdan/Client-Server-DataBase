import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { AppModule } from './app/app.module'; // Импортируем AppModule

platformBrowserDynamic()
  .bootstrapModule(AppModule) // Загружаем AppModule
  .catch((err) => console.error(err));

