CREATE TABLE proprietarios (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    sobrenome VARCHAR(50) NOT NULL,
    data_nascimento DATE NOT NULL,
    sexo CHAR(1) NOT NULL,
    cpf VARCHAR(11),
    email VARCHAR(100) NOT NULL,
    telefone VARCHAR(20) NOT NULL
);

CREATE TABLE especies (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL
);

CREATE TABLE animais (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(50) NOT NULL,
    data_nascimento DATE NOT NULL,
    sexo CHAR(1) NOT NULL,
    raca VARCHAR(50) NOT NULL,
    peso DECIMAL(5,2) NOT NULL,
    vivo BOOLEAN NOT NULL,
    id_proprietario INT NOT NULL,
    id_especie INT NOT NULL,
    FOREIGN KEY (id_proprietario) REFERENCES proprietarios(id),
    FOREIGN KEY (id_especie) REFERENCES especies(id)
);

CREATE TABLE cartilhas_vacinacao (
    id SERIAL PRIMARY KEY,
    nome_vacina VARCHAR(100) NOT NULL,
    descricao_vacina VARCHAR(1000) NOT NULL,
    dose VARCHAR(50) NOT NULL,
    faixa_etaria VARCHAR(50) NOT NULL,
    id_especie INT NOT NULL,
    FOREIGN KEY (id_especie) REFERENCES especies(id)
);

CREATE TABLE vacinas (
    id SERIAL PRIMARY KEY,
    data_administracao DATE NOT NULL,
    lote VARCHAR(50) NOT NULL,
    fabricante VARCHAR(100) NOT NULL,
    data_fabricacao DATE NOT NULL,
    id_animal INT NOT NULL,
    id_cartilha_vacinacao INT NOT NULL,
    FOREIGN KEY (id_animal) REFERENCES animais(id),
    FOREIGN KEY (id_cartilha_vacinacao) REFERENCES cartilhas_vacinacao(id)
);

CREATE TABLE agendas (
    id SERIAL PRIMARY KEY,
    data_hora TIMESTAMP NOT NULL,
    tempo_lembrete SMALLINT NOT NULL,
    id_animal INT NOT NULL,
    id_cartilha_vacinacao INT NOT NULL,
    FOREIGN KEY (id_animal) REFERENCES animais(id),
    FOREIGN KEY (id_cartilha_vacinacao) REFERENCES cartilhas_vacinacao(id)
);

CREATE TABLE usuarios (
    id SERIAL PRIMARY KEY,
    username VARCHAR(100) NOT NULL,
    senha VARCHAR(100) NOT NULL,
    id_proprietario INT NOT NULL,
    FOREIGN KEY (id_proprietario) REFERENCES proprietarios(id)
);
