

SELECT a.title, a.id, a.price, aa.quantity FROM available_assortiments aa, assortiments a WHERE aa.id_assortiment = a.id AND aa.id_cafe = (SELECT id_cafe FROM garcons WHERE id = 2);
