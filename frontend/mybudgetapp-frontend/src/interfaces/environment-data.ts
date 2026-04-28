export interface EnvironmentData {
  production: boolean;

  /** Assumed to end with a trailing '/'. */
  apiBaseURL: string;
  apiVersionParam: string;
  apiVersion: string;
}
