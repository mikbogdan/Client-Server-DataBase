import { Component, OnInit } from '@angular/core';
import { NewsService, Article } from '../services/news.service';

@Component({
  selector: 'app-news-list',
  standalone: false,
  template: `
   <div>
  <!-- Кнопки управления -->
  <button (click)="startCrawling()" [disabled]="isLoading">
    {{ isLoading ? 'Загрузка...' : 'Добавить статьи' }}
  </button>
  <button (click)="loadArticles()">Отобразить все статьи</button>

  <div style="margin-top: 10px;">
    <h2>Поиск по сущностям</h2>
    <input type="text" [(ngModel)]="searchQuery" placeholder="Введите сущность" />
    <button (click)="searchEntities()">Найти</button>
  </div>

  <!-- Общая таблица для всех данных -->
  <div style="margin-top: 20px;">
    <h1>Список новостей</h1>
    <table border="1" style="width: 100%; border-collapse: collapse;">
      <thead>
        <tr>
          <th>Заголовок</th>
          <th>Дата публикации</th>
        </tr>
      </thead>
      <tbody>
        <tr *ngFor="let article of displayedArticles" (click)="selectArticle(article)">
          <td>{{ article.articlE_TITLE }}</td>
          <td>{{ article.articlE_DATA }}</td>
        </tr>
      </tbody>
    </table>
  </div>

  <!-- Детали выбранной статьи -->
  <div *ngIf="selectedArticle" style="margin-top: 20px;">
    <h2>Выбранная статья:</h2>
    <h3>{{ selectedArticle.articlE_TITLE }}</h3>
    <p><strong>Дата публикации:</strong> {{ selectedArticle.articlE_DATA }}</p>
    <p><strong>Текст статьи:</strong></p>
    <p>{{ selectedArticle.articlE_TEXT }}</p>
    <p><strong>Ссылка на статью:</strong></p>
    <p>{{ selectedArticle.articlE_URL }}</p>
  </div>
</div>
  `,
})
export class NewsListComponent implements OnInit {
  articles: Article[] = [];
  selectedArticle: Article | null = null;
  isLoading: boolean = false;
  searchQuery: string = '';
  searchResults: Article[] = [];
  displayedArticles: Article[] = []; 

  constructor(private newsService: NewsService) { }

  ngOnInit(): void {
    this.loadArticles();
  }

  loadArticles(): void {
    this.newsService.getArticles().subscribe(
      (data) => {
        console.log('Полученные статьи:', data);
        this.articles = data || [];
        this.displayedArticles = this.articles; 
        this.isLoading = false;
      },
      (error) => {
        console.error('Ошибка при загрузке статей:', error);
        this.isLoading = false;
      }
    );
  }

  startCrawling(): void {
    this.isLoading = true;
    this.newsService.startCrawling().subscribe(
      () => {
        console.log('Парсинг завершен');
        this.loadArticles();
      },
      (error) => {
        console.error('Ошибка при запуске парсинга:', error);
      }
    );
  }
  
  searchEntities(): void {
    if (!this.searchQuery || !this.searchQuery.trim()) {
      alert('Введите запрос для поиска.');
      return;
    }

    const Query = this.searchQuery.trim();

    this.newsService.searchEntities(Query).subscribe(
      (data) => {
        this.searchResults = data || [];
        this.displayedArticles = this.searchResults;
      },
      (error) => {
        console.error('Ошибка при поиске:', error);
      }
    );
  }

  selectArticle(article: Article): void {
    this.selectedArticle = article;
  }
  

}
