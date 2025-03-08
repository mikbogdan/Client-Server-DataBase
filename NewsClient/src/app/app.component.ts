import { Component } from '@angular/core';


@Component({
  selector: 'app-root',
  standalone: false,
  template: `
    <h1>Новостной парсер</h1>
    <app-news-list></app-news-list> 
  `,
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'NewsClient';
}
