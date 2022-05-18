import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Constants } from '../constants';
import { UserLogin } from '../models/user-login';
import { UserRegistration } from '../models/user-registration';
import { LocalStorageService } from './local-storage.service';
import { TokenHelperService } from './token-helper.service';

@Injectable({
  providedIn: 'root'
})
export class AccountService {

  constructor(
    private http: HttpClient,
    private tokenHelperService: TokenHelperService,
    private localStorageService: LocalStorageService) { }

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

  getCurrentUserId(): string {
    return this.tokenHelperService.getUserId();
  }

  logOut(): void {
    this.localStorageService.remove(Constants.TokenInfo);
  }
}
