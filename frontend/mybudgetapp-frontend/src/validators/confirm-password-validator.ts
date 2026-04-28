import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export function confirmPasswordValidator(
  passwordField: string,
  confirmPasswordField: string,
  noMatchError: string,
): ValidatorFn {
  return (group: AbstractControl): ValidationErrors | null => {
    const password = group.get(passwordField)?.value;
    const confirmPassword = group.get(confirmPasswordField)?.value;

    if (confirmPassword === password) {
      return null;
    } else {
      return { [noMatchError]: true };
    }
  };
}
