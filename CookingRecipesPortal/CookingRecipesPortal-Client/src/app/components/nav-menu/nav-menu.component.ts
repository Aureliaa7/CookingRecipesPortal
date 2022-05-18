import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AccountService } from '../../services/account.service';
import { TokenHelperService } from '../../services/token-helper.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  isExpanded = false;

  constructor(
    private tokenHelperService: TokenHelperService,
    private accountService: AccountService,
    private router: Router) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  isUserAuthenticated(): boolean {
    return !this.tokenHelperService.isTokenExpired();
  }

  logOut() {
    this.accountService.logOut();
    this.router.navigate(['/'])
  }
}
