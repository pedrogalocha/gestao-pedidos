import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTabsModule } from '@angular/material/tabs';
import { AllOrdersComponent } from '../all-orders/all-orders.component';
import { CreateOrderComponent } from '../create-order/create-order.component';
import { ConsultasComponent } from '../consultas/consultas.component';
import { CreateClientComponent } from '../create-client/create-client.component';

@Component({
  selector: 'app-pedidos-dashboard',
  templateUrl: './pedidos-dashboard.component.html',
  standalone: true,
  imports: [CommonModule, MatTabsModule, AllOrdersComponent, CreateOrderComponent, ConsultasComponent, CreateClientComponent]
})
export class PedidosDashboardComponent {
}