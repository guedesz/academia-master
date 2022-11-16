var url = "http://localhost:3000/";
function cadastrarPlano() {
  //construcao do json que vai no body da criacao de usuario

  let body = {
    nomePlano: document.getElementById("nome").value,
    valorPlano: document.getElementById("valor").value,
    tipoPlano: document.getElementById("tipo").value,
  };

  //envio da requisicao usando a FETCH API

  //configuracao e realizacao do POST no endpoint "usuarios"
  fetch(url + "plano/cadastrar", {
    method: "POST",
    redirect: "follow",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: JSON.stringify(body),
  })
    //checa se requisicao deu certo
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    //trata resposta
    .then((output) => {
      console.log(output);
      alert("Cadastro efetuado! :D");
    })
    //trata erro
    .catch((error) => {
      console.log(error);
      alert("Não foi possível efetuar o cadastro! :(");
    });
}
function cadastrarAssinatura() {
  const cpf = document.getElementById("cpf").value;
  let body = {
    dataInicio: document.getElementById("data-inicio").value,
    idPlano: document.getElementById("tipo").value,
  };

  fetch(url + "assinatura/cadastrar/" + cpf, {
    method: "POST",
    redirect: "follow",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: JSON.stringify(body),
  })
    //checa se requisicao deu certo
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    //trata resposta
    .then((output) => {
      console.log(output);
      alert("Cadastro efetuado! :D");
    })
    //trata erro
    .catch((error) => {
      console.log(error);
      alert("Não foi possível efetuar o cadastro! :(");
    });
}

function cadastrarCliente() {
  //validacao de alguns dos inputs
  if (!validaCPF("cpf")) {
    return;
  }

  let body = {
    nome: document.getElementById("nome").value,
    email: document.getElementById("email").value,
    telefone: document.getElementById("telefone").value,
    cpf: document.getElementById("cpf").value,
    dataNascimento: document.getElementById("nascimento").value,
  };

  //envio da requisicao usando a FETCH API

  //configuracao e realizacao do POST no endpoint "usuarios"
  fetch(url + "cliente/cadastrar", {
    method: "POST",
    redirect: "follow",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: JSON.stringify(body),
  })
    //checa se requisicao deu certo
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    //trata resposta
    .then((output) => {
      console.log(output);
      alert("Cadastro efetuado! :D");
    })
    //trata erro
    .catch((error) => {
      console.log(error);
      alert("Não foi possível efetuar o cadastro! :(");
    });
}

function validaCPF(id) {
  let divCPF = document.getElementById(id);
  if (divCPF.value.length == 11) {
    alert("cpf válido");
    return true;
  } else {
    alert("cpf inválido");
    return false;
  }
}

function converterData(data) {
  return data.split("T")[0];
}
function listarAssinaturas() {
  fetch(url + "assinaturas")
    .then((response) => response.json())
    .then((assinaturas) => {
      let listaAssinaturas = document.getElementById("pagina-inicial");

      while (listaAssinaturas.firstChild) {
        listaAssinaturas.removeChild(listaAssinaturas.firstChild);
      }

      for (let assinatura of assinaturas) {
        let divAssinatura = document.createElement("div");
        divAssinatura.setAttribute("class", "form");

        let divNomeAssinante = document.createElement("span");
        divNomeAssinante.placeholder = "Nome do assinante";
        divNomeAssinante.innerHTML = assinatura.nomeCliente;
        divAssinatura.appendChild(divNomeAssinante);

        let divNomePlano = document.createElement("span");
        divNomePlano.placeholder = "Nome do plano";
        divNomePlano.setAttribute("id", assinatura.nomePlano);
        divNomePlano.innerHTML = assinatura.nomePlano;
        divAssinatura.appendChild(divNomePlano);

        let divExpirarAssinatura = document.createElement("input");
        divExpirarAssinatura.placeholder = "Data de expiração";
        divExpirarAssinatura.value = converterData(assinatura.dataExpirar);
        divAssinatura.appendChild(divExpirarAssinatura);

        let btnRemover = document.createElement("button");
        btnRemover.innerHTML = "Remover";
        btnRemover.onclick = (u) => removerAssinatura(assinatura.id);
        btnRemover.style.marginRight = "5px";

        let btnAtualizar = document.createElement("button");
        btnAtualizar.innerHTML = "Atualizar";
        btnAtualizar.onclick = (u) =>
          atualizarAssinatura(
            assinatura.id,
            divExpirarAssinatura,
            divNomePlano
          );
        btnAtualizar.style.marginLeft = "5px";

        let divBotoes = document.createElement("div");
        divBotoes.style.display = "flex";
        divBotoes.appendChild(btnRemover);
        divBotoes.appendChild(btnAtualizar);
        divAssinatura.appendChild(divBotoes);

        listaAssinaturas.appendChild(divAssinatura);
      }
    });
}

function atualizarAssinatura(id, divExpirarAssinatura) {
  let body = {
    dataExpirar: divExpirarAssinatura.value,
  };

  fetch(url + "assinatura/atualizar/" + id, {
    method: "POST",
    redirect: "follow",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: JSON.stringify(body),
  })
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    .then((output) => {
      listarAssinaturas();
      console.log(output);
      alert("Assinatura atualizada!");
    })
    .catch((error) => {
      console.log(error);
      alert("Não foi possível atualizar a assinatura!");
    });
}

function removerAssinatura(id) {
  fetch(url + "assinatura/deletar/" + id, {
    method: "POST",
    redirect: "follow",
  })
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    .then((output) => {
      listarAssinaturas();
      console.log(output);
      alert("assinatura removida com sucesso!");
    })
    .catch((error) => {
      console.log(error);
      alert("Não foi possível remover esta assinatura!");
    });
}

function listarPlanos() {
  fetch(url + "planos")
    .then((response) => response.json())
    .then((planos) => {
      let listaPlanos = document.getElementById("pagina-inicial");

      while (listaPlanos.firstChild) {
        listaPlanos.removeChild(listaPlanos.firstChild);
      }

      for (let plano of planos) {
        let divPlanos = document.createElement("div");
        divPlanos.setAttribute("class", "form");

        let divNomePlano = document.createElement("input");
        divNomePlano.placeholder = "Nome do plano";
        divNomePlano.value = plano.nomePlano;
        divPlanos.appendChild(divNomePlano);

        let divTipoPlano = document.createElement("input");
        divTipoPlano.placeholder = "Tipo do plano";
        divTipoPlano.value = plano.tipoPlano;
        divPlanos.appendChild(divTipoPlano);

        let divValorPlano = document.createElement("input");
        divValorPlano.placeholder = "Tipo do plano";
        divValorPlano.value = plano.valorPlano;
        divPlanos.appendChild(divValorPlano);

        let btnRemover = document.createElement("button");
        btnRemover.innerHTML = "Remover";
        btnRemover.onclick = (u) => removerPlano(plano.id);
        btnRemover.style.marginRight = "5px";

        let btnAtualizar = document.createElement("button");
        btnAtualizar.innerHTML = "Atualizar";
        btnAtualizar.onclick = (u) =>
          atualizarPlano(plano.id, divNomePlano, divTipoPlano, divValorPlano);
        btnAtualizar.style.marginLeft = "5px";

        let divBotoes = document.createElement("div");
        divBotoes.style.display = "flex";
        divBotoes.appendChild(btnRemover);
        divBotoes.appendChild(btnAtualizar);
        divPlanos.appendChild(divBotoes);

        listaPlanos.appendChild(divPlanos);
      }
    });
}

function atualizarPlano(id, divNomePlano, divTipoPlano, divValorPlano) {
  let body = {
    nome: divNomePlano.value,
    tipoPlano: divTipoPlano.value,
    valorPlano: divValorPlano.value,
  };

  fetch(url + "plano/atualizar/" + id, {
    method: "POST",
    redirect: "follow",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: JSON.stringify(body),
  })
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    .then((output) => {
      listarPlanos();
      console.log(output);
      alert("Plano atualizado!");
    })
    .catch((error) => {
      console.log(error);
      alert("Não foi possível atualizar o plano!");
    });
}

function removerPlano(id) {
  fetch(url + "plano/deletar/" + id, {
    method: "POST",
    redirect: "follow",
  })
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    .then((output) => {
      listarPlanos();
      console.log(output);
      alert("Plano removido com sucesso!");
    })
    .catch((error) => {
      console.log(error);
      alert("Não foi possível remover esse plano!");
    });
}

function listarCliente() {
  fetch(url + "clientes")
    .then((response) => response.json())
    .then((clientes) => {
      let listaClientes = document.getElementById("pagina-inicial");

      while (listaClientes.firstChild) {
        listaClientes.removeChild(listaClientes.firstChild);
      }

      for (let cliente of clientes) {
        let divCliente = document.createElement("div");
        divCliente.setAttribute("class", "form");

        //pega o nome do usuario
        let divNome = document.createElement("input");
        divNome.placeholder = "Nome Completo";
        divNome.value = cliente.nome;
        divCliente.appendChild(divNome);

        let divDataNascimento = document.createElement("input");
        divDataNascimento.placeholder = "Data nascimento";
        divDataNascimento.value = converterData(cliente.dataNascimento);
        divCliente.appendChild(divDataNascimento);

        //pega o email do usuario
        let divEmail = document.createElement("input");
        divEmail.placeholder = "Email";
        divEmail.value = cliente.email;
        divCliente.appendChild(divEmail);

        //pega o cpf do usuario
        let divCpf = document.createElement("input");
        divCpf.placeholder = "CPF";
        divCpf.value = cliente.cpf;
        divCliente.appendChild(divCpf);

        let divTelefone = document.createElement("input");
        divTelefone.placeholder = "Telefone";
        divTelefone.value = cliente.telefone;
        divCliente.appendChild(divTelefone);

        //cria o botao para remover o usuario
        let btnRemover = document.createElement("button");
        btnRemover.innerHTML = "Remover";
        btnRemover.onclick = (u) => removerCliente(cliente.id);
        btnRemover.style.marginRight = "5px";

        //cria o botao para atualizar o usuario
        let btnAtualizar = document.createElement("button");
        btnAtualizar.innerHTML = "Atualizar";
        btnAtualizar.onclick = (u) =>
          atualizarCliente(
            cliente.id,
            divNome,
            divEmail,
            divCpf,
            divTelefone,
            divDataNascimento
          );
        btnAtualizar.style.marginLeft = "5px";

        //cria a div com os dois botoes
        let divBotoes = document.createElement("div");
        divBotoes.style.display = "flex";
        divBotoes.appendChild(btnRemover);
        divBotoes.appendChild(btnAtualizar);
        divCliente.appendChild(divBotoes);

        //insere a div do usuario na div com a lista de usuarios
        listaClientes.appendChild(divCliente);
      }
    });
}

function atualizarCliente(
  id,
  divNome,
  divEmail,
  divCpf,
  divTelefone,
  divNascimento
) {
  let body = {
    nome: divNome.value,
    email: divEmail.value,
    cpf: divCpf.value,
    telefone: divTelefone.value,
    dataNascimento: divNascimento.value,
  };

  fetch(url + "cliente/atualizar/" + id, {
    method: "POST",
    redirect: "follow",
    headers: {
      "Content-Type": "application/json",
      Accept: "application/json",
    },
    body: JSON.stringify(body),
  })
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    .then((output) => {
      listarCliente();
      console.log(output);
      alert("Usuário atualizado!");
    })
    .catch((error) => {
      console.log(error);
      alert("Não foi possível atualizar o usuário!");
    });
}

function removerCliente(id) {
  fetch(url + "cliente/deletar/" + id, {
    method: "POST",
    redirect: "follow",
  })
    .then((response) => {
      if (response.ok) {
        return response.text();
      } else {
        return response.text().then((text) => {
          throw new Error(text);
        });
      }
    })
    .then((output) => {
      listarCliente();
      console.log(output);
      alert("Cliente removido com sucesso!");
    })
    .catch((error) => {
      console.log(error);
      alert("Não foi possível remover esse cliente!");
    });
}
