import { inject } from "@angular/core";
import { CanActivateFn } from "@angular/router";
import { AccountService } from "../_services/account.service";
import { ToastrService } from "ngx-toastr";
import { map } from "rxjs";




export const adminGuard: CanActivateFn = (route,state) => {
  const accountService = inject(AccountService);
  const toastr = inject(ToastrService);

  return accountService.currentUser$.pipe(
    map(user => {
      if(!user) return false;
      if(user.roles.includes('Admin') || user.roles.includes('Moderator')){
        return true;
      }else{
        toastr.error('You cannot enter in this area');
        return false;
      }
    })
  )

};


// import { Injectable } from '@angular/core';
// import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
// import { Observable } from 'rxjs';
// @Injectable({
//   providedIn: 'root'
// })
// export class AdminGuard implements CanActivate {
//   canActivate(
//     route: ActivatedRouteSnapshot,
//     state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
//     return true;
//   }
  
// }
