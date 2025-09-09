import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApiService, Cliente } from '../../services/api.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSelectModule } from '@angular/material/select';

@Component({
  selector: 'app-create-order',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatSelectModule
  ],
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css']
})
export class CreateOrderComponent implements OnInit {
  clienteId: number | null = null;
  clientes: Cliente[] = [];
  itens: { produto: string; quantidade: number; precoUnitario: number }[] = [{ produto: '', quantidade: 1, precoUnitario: 0 }];
  successMessage = '';
  errorMessage = '';

  constructor(private apiService: ApiService) {}

  ngOnInit(): void {
    this.apiService.getAllClientes().subscribe(clientes => {
      this.clientes = clientes;
    });
  }

  addItem() {
    this.itens.push({ produto: '', quantidade: 1, precoUnitario: 0 });
  }

  removeItem(index: number) {
    this.itens.splice(index, 1);
  }

  createOrder() {
    if (this.clienteId && this.itens.length > 0) {
      const pedido = {
        clienteId: this.clienteId,
        itens: this.itens.map(item => ({
          produto: item.produto,
          quantidade: item.quantidade,
          precoUnitario: item.precoUnitario
        }))
      };
      this.apiService.enfileirarPedido(pedido).subscribe({
        next: () => {
          this.successMessage = 'Pedido criado com sucesso!';
          this.errorMessage = '';
          this.resetForm();
        },
        error: (err: any) => {
          this.errorMessage = 'Erro ao criar pedido.';
          this.successMessage = '';
          console.error(err);
        }
      });
    }
  }

  resetForm() {
    this.clienteId = null;
    this.itens = [{ produto: '', quantidade: 1, precoUnitario: 0 }];
  }
}
