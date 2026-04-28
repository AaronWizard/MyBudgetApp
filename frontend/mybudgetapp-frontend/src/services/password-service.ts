import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { PasswordRequirements } from '../interfaces/password-requirements';
import { environment } from '../environments/environment';
import { baseHeader } from './data/api-version-header';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class PasswordService {
  private readonly methodPasswordRequirements = 'user/password/requirements';

  private http = inject(HttpClient);

  getRequirements(): Observable<PasswordRequirements> {
    return this.http.get<PasswordRequirements>(
      environment.apiBaseURL + this.methodPasswordRequirements,
      { headers: baseHeader },
    );
  }
}
