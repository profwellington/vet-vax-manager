-- PROPRIETARIOS
INSERT INTO proprietarios (nome, sobrenome, data_nascimento, sexo, cpf, email, telefone) VALUES
('João', 'Silva', '1990-03-15', 'M', '12345678901', 'joaosilva@email.com', '11987654321'),
('Maria', 'Souza', '1985-08-20', 'F', '98765432109', 'mariasouza@email.com', '21123456789'),
('Pedro', 'Santos', '1978-05-10', 'M', '45678901234', 'pedrosantos@email.com', '31555555555');

-- USUARIOS
INSERT INTO usuarios (username, senha, id_proprietario) VALUES
('joao.silva', '$2a$11$e/lfiGffuE14AtSm.yaUyeSc04YPCvvWmrmlmp3ksWqXVR.OOve', 1), -- SENHA: 123456
('mariasouza', '$2a$11$e/lfiGffuE14AtSm.yaUyeSc04YPCvvWmrmlmp3ksWqXVR.OOve', 2), -- SENHA: 123456
('pedro-santos', '$2a$11$e/lfiGffuE14AtSm.yaUyeSc04YPCvvWmrmlmp3ksWqXVR.OOve', 3); -- SENHA: 123456

-- ESPECIES
INSERT INTO especies (nome) VALUES ('Cachorro'), ('Gato'), ('Cavalo');

-- ANIMAIS
INSERT INTO animais (nome, data_nascimento, sexo, raca, peso, vivo, id_proprietario, id_especie) VALUES
('Totó', '2019-04-10', 'M', 'SRD', 8.50, TRUE, 1, 1),
('Frajola', '2020-01-15', 'M', 'Siames', 5.50, TRUE, 2, 2),
('Pé de pano', '2018-08-05', 'M', 'Puro-sangue inglês', 4.5, TRUE, 3, 3),
('Amora', '2017-12-20', 'F', 'Persa', 1.0, TRUE, 2, 2),
('Pandora', '2019-06-25', 'F', 'Golden Retriever', 15.70, TRUE, 1, 1);
