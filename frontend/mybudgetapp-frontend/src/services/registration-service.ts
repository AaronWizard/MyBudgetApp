import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../environments/environment';
import { baseHeader } from './data/api-version-header';
import { catchError, Observable, of, map } from 'rxjs';

interface RegistrationData {
  email: string;
  password: string;
}

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  private readonly methodRegister = 'user/register';
  private readonly methodVerify = 'user/register/verify';

  private http = inject(HttpClient);

  register(email: string, password: string): Observable<boolean> {
    const data: RegistrationData = { email: email, password: password };
    return this.http
      .post(environment.apiBaseURL + this.methodRegister, data, { headers: baseHeader })
      .pipe(
        map(() => true),
        catchError((error) => {
          console.error(error);
          return of(false);
        }),
      );
  }

  verify(token: string) {
    const params = new HttpParams().set;
    this.http
      .post(`${environment.apiBaseURL}${this.methodVerify}/${token}`, null, {
        headers: baseHeader,
      })
      .subscribe({
        error: (error) => console.error(error),
      });
  }
}
