

using Microsoft.EntityFrameworkCore;

public class ContratoService
{
    private readonly AppDbContext _context;
    public ContratoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ContratoResponse> PostContrato(ContratoRequest contratoRequest)
    {
        var clienteExistente = await _context.Clientes.AnyAsync(c => c.Id == contratoRequest.ClienteId);
        if(!clienteExistente)
        {
            throw new BusinessException
            (
                "Cliente não encontrado no sistema", 404
            );
        }

        var numeroExistente = await _context.Contratos.AnyAsync(c => c.Numero == contratoRequest.Numero);
        if (numeroExistente)
        {
            throw new BusinessException(
                "Já existe um contrato com este número.",
                409
            );
        }

        if (contratoRequest.DataFim <= contratoRequest.DataInicio)
        {
            throw new BusinessException(
                "A data de término deve ser posterior à data de início."
            );
        }

        var contrato = new Contrato
        {
            ClienteId = contratoRequest.ClienteId,
            Numero = contratoRequest.Numero,
            Titulo = contratoRequest.Titulo,
            Descricao = contratoRequest.Descricao,
            Valor = contratoRequest.Valor,
            DataInicio = contratoRequest.DataInicio,
            DataFim = contratoRequest.DataFim
        };

        await _context.Contratos.AddAsync(contrato);

        await _context.SaveChangesAsync();

        var response = new ContratoResponse
        {
            Id = contrato.Id,
            ClienteId = contrato.ClienteId,
            Numero = contrato.Numero,
            Titulo = contrato.Titulo,
            Descricao = contrato.Descricao,
            Valor = contrato.Valor,
            DataInicio = contrato.DataInicio,
            DataFim = contrato.DataFim,
            Status = contrato.Status,
            CriadoEm = contrato.CriadoEm
        };

        return response;

    }
    public async Task<List<ContratoResponse>> GetContratos()
    {
        return await _context.Contratos
            .AsNoTracking()
            .Select(c => new ContratoResponse
            {
                Id = c.Id,
                ClienteId = c.ClienteId,
                Numero = c.Numero,
                Titulo = c.Titulo,
                Descricao = c.Descricao,
                Valor = c.Valor,
                DataInicio = c.DataInicio,
                DataFim = c.DataFim,
                Status = c.Status,
                CriadoEm = c.CriadoEm
            })
            .ToListAsync();
    }
    public async Task<ContratoResponse?> GetContratoId(int id)
    {
        var contrato = await _context.Contratos
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contrato == null)
        {
            return null;
        }

        return MapearParaResponse(contrato);
    }
    public async Task<ContratoResponse?> PutContrato(
        int id,
        ContratoUpdateRequest contratoRequest)
    {
        var contrato = await _context.Contratos
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contrato == null)
        {
            return null;
        }

        if (contrato.Status != "RASCUNHO")
        {
            throw new BusinessException(
                "Apenas contratos em RASCUNHO podem ser alterados."
            );
        }

        if (contratoRequest.DataFim <= contratoRequest.DataInicio)
        {
            throw new BusinessException(
                "A data de término deve ser posterior à data de início."
            );
        }

        contrato.Titulo = contratoRequest.Titulo;
        contrato.Descricao = contratoRequest.Descricao;
        contrato.Valor = contratoRequest.Valor;
        contrato.DataInicio = contratoRequest.DataInicio;
        contrato.DataFim = contratoRequest.DataFim;

        await _context.SaveChangesAsync();

        return MapearParaResponse(contrato);
    }
    public async Task<ContratoResponse?> AlterarStatus(
        int id,
        ContratoStatusRequest contratoRequest)
    {
        var contrato = await _context.Contratos
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contrato == null)
        {
            return null;
        }

        var transicoesPermitidas = new Dictionary<string, string[]>
        {
            ["RASCUNHO"] = new[] { "ATIVO", "CANCELADO" },
            ["ATIVO"] = new[] { "ENCERRADO", "CANCELADO" }
        };

        if (!transicoesPermitidas.TryGetValue(
                contrato.Status,
                out var proximosStatus))
        {
            throw new BusinessException(
                $"Não é possível alterar o contrato que está com status {contrato.Status}."
            );
        }

        if (!proximosStatus.Contains(contratoRequest.Status))
        {
            throw new BusinessException(
                $"Não é permitido alterar o contrato de {contrato.Status} para {contratoRequest.Status}."
            );
        }

        contrato.Status = contratoRequest.Status;

        await _context.SaveChangesAsync();

        return MapearParaResponse(contrato);
    }
    public async Task<bool> DeleteContrato(int id)
    {
        var contrato = await _context.Contratos
            .FirstOrDefaultAsync(c => c.Id == id);

        if (contrato == null)
        {
            return false;
        }

        if (contrato.Status == "ENCERRADO")
        {
            throw new BusinessException(
                "Um contrato encerrado não pode ser cancelado."
            );
        }

        if (contrato.Status == "CANCELADO")
        {
            throw new BusinessException(
                "O contrato já está cancelado."
            );
        }

        contrato.Status = "CANCELADO";

        await _context.SaveChangesAsync();

        return true;
    }


    // Conversão Entity → Response
    private static ContratoResponse MapearParaResponse(
        Contrato contrato)
    {
        return new ContratoResponse
        {
            Id = contrato.Id,
            ClienteId = contrato.ClienteId,
            Numero = contrato.Numero,
            Titulo = contrato.Titulo,
            Descricao = contrato.Descricao,
            Valor = contrato.Valor,
            DataInicio = contrato.DataInicio,
            DataFim = contrato.DataFim,
            Status = contrato.Status,
            CriadoEm = contrato.CriadoEm
        };
    }
}

