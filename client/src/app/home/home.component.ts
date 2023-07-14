import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  registerMode = false;
  users: any;

  constructor() { }   // private http: HttpClient - removed

  ngOnInit(): void {
    // this.getUsers();
  }

  registerToggle(){
    this.registerMode = !this.registerMode;
  }

  // getUsers(){
  //   this.http.get('https://localhost:5001/api/Users').subscribe({   
  //     //callback functions
  //     next: Response => this.users = Response,
  //     //error: () => {},
  //     error: error => console.log(error),
  //     complete: () => console.log('Request has completed')   
  //    })
  // }   //to observe an  observable need to subscribe it

  cancelRegisterMode(event: boolean){
    this.registerMode = event;
  }

}
