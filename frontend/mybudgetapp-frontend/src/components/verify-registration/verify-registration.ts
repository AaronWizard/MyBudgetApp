import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import {
  RegistrationService,
  VerifyRegistrationResponse,
} from '../../services/registration-service';

const State = {
  loading: 'loading',
  missingParams: 'missing-params',
} as const;
type State = (typeof State)[keyof typeof State] | VerifyRegistrationResponse;

@Component({
  selector: 'app-verify-registration',
  imports: [RouterLink],
  templateUrl: './verify-registration.html',
  styleUrl: './verify-registration.scss',
})
export class VerifyRegistration {
  private route = inject(ActivatedRoute);
  private registrationService = inject(RegistrationService);

  state = signal<State>(State.loading);

  protected readonly State = State;
  protected readonly VerifyRegistrationResponse = VerifyRegistrationResponse;

  constructor() {
    const userId = this.route.snapshot.queryParamMap.get('user') ?? '';
    const token = this.route.snapshot.queryParamMap.get('token') ?? '';

    if (!userId || !token) {
      this.state.set(State.missingParams);
    } else {
      this.verifyRegistration(userId, token);
    }
  }

  private verifyRegistration(userId: string, token: string) {
    this.registrationService.verify(userId, token).subscribe((result) => this.state.set(result));
  }
}
