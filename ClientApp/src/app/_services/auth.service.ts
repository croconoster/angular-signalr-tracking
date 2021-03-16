import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

const AUTH_API = 'https://localhost:44317/api/Accounts/'

const httpOptions = {headers : new HttpHeaders({'Content-Type' : 'application/json'})}

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http : HttpClient) { }

  login(email : string, password : string) : Observable<any> {
    return this.http.post(AUTH_API+ 'login', {email, password}, httpOptions);
  }

  register(email: string, password: string, firstname: string, lastname: string) : Observable<any>{
    return this.http.post(AUTH_API + 'create', {email, password, firstname, lastname}, httpOptions)
  }
}
