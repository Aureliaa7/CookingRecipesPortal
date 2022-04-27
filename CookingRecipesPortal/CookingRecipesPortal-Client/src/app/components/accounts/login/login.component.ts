import { Component } from '@angular/core';
import { FormBuilder, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ResponseStatusCodes } from '../../../enums/response-status-codes.enum';
import { UserLogin } from '../../../models/user-login';
import { AccountService } from '../../../services/account.service';
import { TokenHelperService } from '../../../services/token-helper.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  loginForm = this.formBuilder.group({
    email: new FormControl('', [Validators.required, Validators.email]),
    password: new FormControl('', [Validators.required])
  });

  constructor(
    private router: Router,
    private accountService: AccountService,
    private formBuilder: FormBuilder,
    private tokenHelperService: TokenHelperService) { }

  login() {
    if (this.loginForm.invalid) {
      return;
    }

    let userLogin = new UserLogin(this.loginForm.value.email, this.loginForm.value.password);
    this.accountService.login(userLogin).subscribe((response: any) => {
      let token = (<any>response).token;
      if (!token) {
        return;
      }
      this.tokenHelperService.saveToken(token);
      this.router.navigate(["/"]);
    },
      (err: { status: any; }) => {
        if (err.status == ResponseStatusCodes.Unauthorized) {
          this.loginForm.reset();
        }
      });
  }
}


