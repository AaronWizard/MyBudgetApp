import { HttpHeaders } from '@angular/common/http';
import { environment } from '../../environments/environment';

export const baseHeader = new HttpHeaders().set(
  environment.apiVersionParam,
  environment.apiVersion,
);
