import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';
import { environment } from '../environments/environment';
import { baseHeader } from './data/api-version-header';
import { catchError, Observable, of, map, throwError } from 'rxjs';

interface RegistrationRequest {
  email: string;
  password: string;
}

// Response from the service.
interface RegistrationResponse {
  success: boolean;
  invalidEmail?: boolean;
  passwordErrors?: string[];
}

// Response from the API called by the service.
interface RawRegistrationResponse {
  invalidEmail: boolean;
  passwordErrors: string[];
}

interface VerifyRegistrationRequest {
  userId: string;
  token: string;
}

export const VerifyRegistrationResponse = {
  success: 'success',
  notFound: 'not-found',
  alreadyVerified: 'already-verified',
  invalidToken: 'invalid-token',
  serverError: 'server-error',
} as const;
export type VerifyRegistrationResponse =
  (typeof VerifyRegistrationResponse)[keyof typeof VerifyRegistrationResponse];

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  private readonly methodRegister = 'user/register';
  private readonly methodVerify = 'user/register/verify';

  private http = inject(HttpClient);

  register(email: string, password: string): Observable<RegistrationResponse> {
    const requestBody: RegistrationRequest = { email: email, password: password };
    return this.http
      .post(environment.apiBaseURL + this.methodRegister, requestBody, { headers: baseHeader })
      .pipe(
        map(() => ({ success: true }) as RegistrationResponse),
        catchError((error: HttpErrorResponse) => {
          if (error.status === 400) {
            const registrationError = error.error as RawRegistrationResponse;
            return of({
              success: false,
              invalidEmail: registrationError.invalidEmail,
              passwordErrors: registrationError.passwordErrors,
            } as RegistrationResponse);
          } else {
            return throwError(() => error); // Re-throw unexpected errors
          }
        }),
      );
  }

  verify(userId: string, token: string): Observable<VerifyRegistrationResponse> {
    const requestBody: VerifyRegistrationRequest = { userId: userId, token: token };
    return this.http
      .post(environment.apiBaseURL + this.methodVerify, requestBody, {
        headers: baseHeader,
      })
      .pipe(
        map(() => VerifyRegistrationResponse.success),
        catchError((error: HttpErrorResponse) => {
          let result: VerifyRegistrationResponse = VerifyRegistrationResponse.serverError;
          switch (error.status) {
            case 404:
              result = VerifyRegistrationResponse.notFound;
              break;
            case 409:
              result = VerifyRegistrationResponse.alreadyVerified;
              break;
            case 400:
              result = VerifyRegistrationResponse.invalidToken;
              break;
          }
          return of(result);
        }),
      );
  }
}
