import { Component, OnInit } from '@angular/core';
import { AccountService } from '../_services/account.service';
import { Observable, of } from 'rxjs';
import { User } from '../_models/user';
import { Route, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

model : any =  {};
// loggedIn = false;
// currentUser$ : Observable<User | null> = of(null);           // removed and directly used

  constructor(public accountService: AccountService,private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
   // this.getCurrentUser();
  //  this.currentUser$ = this.accountService.currentUser$;     //directly used
  }

  // getCurrentUser(){
  //   this.accountService.currentUser$.subscribe({
  //     next: user => this.loggedIn = !!user,
  //     error: error => console.log(error)
  //   })
  // }

  login(){
    this.accountService.login(this.model).subscribe({
        next: _ => this.router.navigateByUrl('/members'),      //_ means in there is no any arguments ,used insted of ()

        // error: error => this.toastr.error(error.error)         //because is going to interceptor now                      //console.log(error)
      })
        //{
        // console.log(response);
       // console.log("this is called");
        //this.loggedIn = true;
     // },
    // console.log(this.model);
  }
  logout(){
   // this.loggedIn = false;
    this.accountService.logout();         //remove item from local storage
    this.router.navigateByUrl('/')
  }

}