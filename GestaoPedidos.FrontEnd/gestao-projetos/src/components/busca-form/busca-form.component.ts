import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { Cliente } from '../../services/api.service';

@Component({
  selector: 'app-busca-form',
  templateUrl: './busca-form.component.html',
  styleUrls: ['./busca-form.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule
  ]
})
export class BuscaFormComponent {
  clienteId: number | null = null;
  pedidoId: number | null = null;

  @Input() clientes: Cliente[] = [];
  @Output() buscarPorCliente = new EventEmitter<number>();
  @Output() buscarPorPedido = new EventEmitter<number>();

  onBuscarPorCliente(): void {
    if (this.clienteId) {
      this.buscarPorCliente.emit(this.clienteId);
    }
  }

  onBuscarPorPedido(): void {
    if (this.pedidoId) {
      this.buscarPorPedido.emit(this.pedidoId);
    }
  }
}