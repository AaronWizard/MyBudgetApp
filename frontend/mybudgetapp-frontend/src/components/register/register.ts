import { Component, signal } from '@angular/core';
import { FormControl, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RouterLink } from '@angular/router';
import { PasswordService } from '../../services/password-service';

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
  registerForm = new FormGroup({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required]),
    confirmPassword: new FormControl('', [Validators.required]),
  });

  readonly loadError = signal(false);

  constructor(private passwordService: PasswordService) {}

  ngOnInit(): void {
    this.passwordService.getPasswordRequirements().subscribe({
      next: (requirements) => {
        console.log(requirements);
      },
      error: (_) => {
        console.error('Error loading password requirements');
        this.loadError.set(true);
      },
    });
  }

  onSubmit() {
    console.log('registering');
  }
}
