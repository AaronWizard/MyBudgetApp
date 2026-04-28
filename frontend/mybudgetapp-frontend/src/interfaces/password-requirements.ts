export interface PasswordRequirements {
  requireDigit: boolean;
  requireLowercase: boolean;
  requireNonAlphanumeric: boolean;
  requireUppercase: boolean;
  requiredLength: number;
}
