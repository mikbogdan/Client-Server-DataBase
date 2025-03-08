import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, switchMap } from 'rxjs';

export interface Article {
  articlE_TITLE: string;
  articlE_URL: string;
  articlE_TEXT: string;
  articlE_DATA: string;
  fulL_HTML: string;
  entities: string;
}

@Injectable({
  providedIn: 'root',
})
export class NewsService {
  private apiUrl = 'http://localhost:5243';
  private query: string = '';
  constructor(private http: HttpClient) {}

  getArticles(): Observable<Article[]> {
    return this.http.get<Article[]>(`${this.apiUrl}/articles`);
  }

  startCrawling(): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/crawl`, {});
  }

  sendSearchQuery(Query: string): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/search`, { Query });
  }

  // Метод для получения результатов
  getSearchResults(): Observable<Article[]> {
    return this.http.get<Article[]>(`${this.apiUrl}/search/results`);
  }

  // Комбинированный метод для поиска
  searchEntities(Query: string): Observable<Article[]> {
    return this.sendSearchQuery(Query).pipe(
      switchMap(() => this.getSearchResults())
    );
  }
  

  
}
