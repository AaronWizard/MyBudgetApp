import { EnvironmentData } from '../interfaces/environment-data';

export const environment: EnvironmentData = {
  production: false,
  apiBaseURL: 'http://localhost:5000/api/',

  apiVersionParam: 'x-api-version',
  apiVersion: '1.0',
};
