import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { RegistrationService } from '../../services/registration-service';

@Component({
  selector: 'app-verify-registration',
  imports: [],
  templateUrl: './verify-registration.html',
  styleUrl: './verify-registration.scss',
})
export class VerifyRegistration {
  private route = inject(ActivatedRoute);
  private registrationService = inject(RegistrationService);

  loading = signal(true);

  constructor() {
    const token = this.route.snapshot.paramMap.get('token') ?? '';
    this.verifyRegistration(token);
  }

  private verifyRegistration(token: string) {
    //this.registrationService.verify(token);
    this.loading.set(false);
  }
}
