import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Constants } from '../constants';
import { LocalStorageService } from './local-storage.service';

@Injectable({
  providedIn: 'root'
})
export class TokenHelperService {
  private jwtService = new JwtHelperService();

  constructor(private storageService: LocalStorageService) { }

  saveToken(token: string) {
    this.storageService.set(Constants.TokenInfo, token);
  }

  getUserId() {
    let decodedToken = this.getDecodedToken();
    return decodedToken !== null ? decodedToken[Constants.UserId] : '';
  }

  getToken() {
    return this.storageService.get(Constants.TokenInfo);
  }

  isTokenExpired() {
    let token = this.storageService.get(Constants.TokenInfo);
    if (this.jwtService.isTokenExpired(token)) {
      this.storageService.clear();
      return true;
    }
    return false;
  }

  private getDecodedToken() {
    let token = this.storageService.get(Constants.TokenInfo);
    return this.jwtService.decodeToken(token);
  }

  removeToken() {
    this.storageService.remove(Constants.TokenInfo);
  }
}
