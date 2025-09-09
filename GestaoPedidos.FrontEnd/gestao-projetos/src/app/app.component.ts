import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { PedidosDashboardComponent } from '../components/pedidos-dashboard/pedidos-dashboard.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, PedidosDashboardComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'gestao-projetos';
}
