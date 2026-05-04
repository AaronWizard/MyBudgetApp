import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';

import { PasswordService } from '../../services/password-service';
import { RegistrationService } from '../../services/registration-service';
import { confirmPasswordValidator } from '../../validators/confirm-password-validator';
import { passwordValidator } from '../../validators/password-validator';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    RouterLink,
  ],
  templateUrl: './register.html',
  styleUrl: './register.scss',
})
export class Register {
  private formBuilder = inject(FormBuilder);

  private passwordService = inject(PasswordService);
  private registrationService = inject(RegistrationService);

  registerForm = this.formBuilder.group(
    {
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
      confirmPassword: ['', Validators.required],
    },
    {
      validators: [confirmPasswordValidator('password', 'confirmPassword', 'nomatch')],
    },
  );

  loading = signal(true);

  successfullyLoaded = signal(false);
  problemRegisteringOccurred = signal(false);
  registrationSent = signal(false);

  ngOnInit(): void {
    this.loading.set(true);
    this.passwordService
      .getPasswordRequirements()
      .pipe(finalize(() => this.loading.set(false)))
      .subscribe({
        next: (requirements) => {
          this.registerForm.controls.password.addValidators(passwordValidator(requirements));
          this.registerForm.controls.password.updateValueAndValidity();

          this.loading.set(false);
          this.successfullyLoaded.set(true);
        },
        error: (_) => {
          console.error('Error loading password requirements');
          this.successfullyLoaded.set(false);
        },
      });
  }

  onSubmit() {
    const email = this.registerForm.value.email ?? '';
    const password = this.registerForm.value.password ?? '';

    if (email && password) {
      this.loading.set(true);
      this.registrationService
        .register(email, password)
        .pipe(finalize(() => this.loading.set(false)))
        .subscribe({
          next: (result) => {
            this.registrationSent.set(result.success);
            this.problemRegisteringOccurred.set(!result.success);
            if (!result.success) {
              console.error(`Error occurred while registering: ${result}`);
            }
          },
        });
    } else {
      console.error('Email and password are empty');
      this.problemRegisteringOccurred.set(true);
    }
  }
}
