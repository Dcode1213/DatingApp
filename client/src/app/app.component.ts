import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'Dating App';
  users : any;

  constructor(private http: HttpClient){      //In the constructor, HttpClient is injected as a dependency. (Dependenct injection)

  }
  ngOnInit(): void {
   // throw new Error('Method not implemented.');
   
   this.http.get('https://localhost:5001/api/Users').subscribe({   
    //callback functions
    next: Response => this.users = Response,
    //error: () => {},
    error: error => console.log(error),
    complete: () => console.log('Request has completed')
     
   })    //to observe an  observable need to subscribe it
  }
}
