import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { UserLogin } from '../models/user-login';
import { UserRegistration } from '../models/user-registration';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(private http: HttpClient) { }

  login(userLogin: UserLogin) {
    const credentials = JSON.stringify(userLogin);
    return this.http.post("/api/accounts/login", credentials, {
      headers: Constants.HeadersContentType
    });
  }

  register(user: UserRegistration) {
    const userJson = JSON.stringify(user);
    return this.http.post("/api/accounts/register", userJson, {
      headers: Constants.HeadersContentType
    });
  }
}
