import { ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { PasswordRequirements } from '../interfaces/password-requirements';

export function passwordValidator(requirements: PasswordRequirements): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value: string = control.value ?? '';
    const errors: ValidationErrors = {};

    if (requirements.requireDigit && !/[0-9]/.test(value)) {
      errors['requireDigit'] = true;
    }
    if (requirements.requireLowercase && !/[a-z]/.test(value)) {
      errors['requireLowercase'] = true;
    }
    if (requirements.requireNonAlphanumeric && !/[^a-zA-Z0-9]/.test(value)) {
      errors['requireNonAlphanumeric'] = true;
    }
    if (requirements.requireUppercase && !/[A-Z]/.test(value)) {
      errors['requireUppercase'] = true;
    }
    if (value.length < requirements.requiredLength) {
      errors['requiredLength'] = requirements.requiredLength;
    }

    if (Object.keys(errors).length > 0) {
      return errors;
    } else {
      return null;
    }
  };
}
