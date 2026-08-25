import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AdminNavComponent } from './shared/components/admin-nav/admin-nav';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    AdminNavComponent
  ],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})
export class App {
}