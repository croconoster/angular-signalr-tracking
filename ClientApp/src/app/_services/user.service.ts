import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { environment } from '@environments/environment';
import { User } from '@app/_models/user';

const API_URL = '';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient) { }


    getAll() {
        return this.http.get<User[]>(`${environment.apiUrl}/api/accounts`);
    }

    getById(id: number) {
        return this.http.get<User>(`${environment.apiUrl}/api/accounts/${id}`);
    }

    register(user : User)  {
      return this.http.post<any>(`${environment.apiUrl}/api/accounts/create`, user);
    }
}
