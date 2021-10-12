CREATE TABLE `CompanyHub_alpha`.`symbols` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `ticker` VARCHAR(15) NULL,
  `exchange` VARCHAR(10) NULL,
  `company` VARCHAR(150) NULL,
  `industry` VARCHAR(150) NULL,
  `totalvalue` DOUBLE NULL,
  PRIMARY KEY (`id`));