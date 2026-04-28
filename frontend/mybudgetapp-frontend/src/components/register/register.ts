import { Component, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';

import { PasswordService } from '../../services/password-service';
import { RegistrationService } from '../../services/registration-service';
import { confirmPasswordValidator } from '../../validators/confirm-password-validator';
import { passwordValidator } from '../../validators/password-validator';

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

  successfullyLoaded = signal(false);

  ngOnInit(): void {
    this.passwordService.getPasswordRequirements().subscribe({
      next: (requirements) => {
        this.registerForm.get('password')?.addValidators(passwordValidator(requirements));
        this.registerForm.updateValueAndValidity();
        this.successfullyLoaded.set(true);
      },
      error: (_) => {
        console.error('Error loading password requirements');
        this.successfullyLoaded.set(false);
      },
    });
  }

  onSubmit() {
    console.log('registering');
  }
}
