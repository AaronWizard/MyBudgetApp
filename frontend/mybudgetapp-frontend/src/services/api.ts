import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Api {
  private readonly baseUrl = 'http://localhost:5000/api/';
  private readonly versionParam = 'x-api-version';
  private readonly version = '1.0';

  private readonly baseHeaders = new HttpHeaders().set(this.versionParam, this.version);

  constructor(private http: HttpClient) {}

  get(method: string): Observable<Object> {
    return this.http.get(this.baseUrl + method, { headers: this.baseHeaders });
  }

  post(method: string, requestBody: any | null): Observable<Object> {
    return this.http.post(this.baseUrl + method, requestBody, { headers: this.baseHeaders });
  }

  put(method: string, requestBody: any | null): Observable<Object> {
    return this.http.put(this.baseUrl + method, requestBody, { headers: this.baseHeaders });
  }

  delete(method: string, requestBody: any | null): Observable<Object> {
    return this.http.delete(this.baseUrl + method, { headers: this.baseHeaders });
  }
}
