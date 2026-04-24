import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../environments/environment';
import { RegistrationData } from '../interfaces/registration-data';
import { baseHeader } from './data/api-version-header';

@Injectable({
  providedIn: 'root',
})
export class RegistrationService {
  private readonly methodRegister = 'register';
  private readonly methodVerify = 'register/verify';

  private http = inject(HttpClient);

  register(data: RegistrationData) {
    this.http.post(environment.apiBaseURL + this.methodRegister, data, { headers: baseHeader });
  }

  verify(token: string) {
    const params = new HttpParams().set;
    this.http.post(`${environment.apiBaseURL}${this.methodVerify}${token}`, null, {
      headers: baseHeader,
    });
  }
}
