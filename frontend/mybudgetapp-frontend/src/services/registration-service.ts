import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { environment } from '../environments/environment';
import { baseHeader } from './data/api-version-header';
import { catchError, Observable, of, map, throwError } from 'rxjs';

interface RegistrationData {
  email: string;
  password: string;
}

interface RegistrationResult {
  success: boolean;
  invalidEmail?: boolean;
  passwordErrors?: string[];
}

interface RegistrationAPIResult {
  invalidEmail: boolean;
  passwordErrors: string[];
}

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  private readonly methodRegister = 'user/register';
  private readonly methodVerify = 'user/register/verify';

  private http = inject(HttpClient);

  register(email: string, password: string): Observable<RegistrationResult> {
    const data: RegistrationData = { email: email, password: password };
    return this.http
      .post(environment.apiBaseURL + this.methodRegister, data, { headers: baseHeader })
      .pipe(
        map(() => ({ success: true }) as RegistrationResult),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            const registrationError = error.error as RegistrationAPIResult;
            return of({
              success: false,
              invalidEmail: registrationError.invalidEmail,
              passwordErrors: registrationError.passwordErrors,
            } as RegistrationResult);
          } else {
            return throwError(() => error); // Re-throw unexpected errors
          }
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
