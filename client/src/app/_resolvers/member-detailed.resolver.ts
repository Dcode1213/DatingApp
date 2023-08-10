// import { Injectable } from '@angular/core';
// import {
//   Router, Resolve,
//   RouterStateSnapshot,
//   ActivatedRouteSnapshot
// } from '@angular/router';
// import { Observable, of } from 'rxjs';

// @Injectable({
//   providedIn: 'root'
// })
// export class MemberDetailedResolver implements Resolve<boolean> {
//   resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
//     return of(true);
//   }
// }

import { ResolveFn } from "@angular/router";
import { Member } from "../_models/member";
import { inject } from "@angular/core";
import { MembersService } from "../_services/members.service";


export const MemberDetailedResolver: ResolveFn<Member> = (route,state) =>{

   const memberService = inject(MembersService);
   return memberService.getMember(route.paramMap.get('username')!);

};