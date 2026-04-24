import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class Api {
  private url = 'http://localhost:5000/api/';
}
