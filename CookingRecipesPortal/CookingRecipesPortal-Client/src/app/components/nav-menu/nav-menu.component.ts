import { Component } from '@angular/core';
import { TokenHelperService } from '../../services/token-helper.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {

  isExpanded = false;

  constructor(private tokenHelperService: TokenHelperService) { }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  isUserAuthenticated(): boolean {
    return !this.tokenHelperService.isTokenExpired();
  }
}
