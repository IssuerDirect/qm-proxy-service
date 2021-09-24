CREATE TABLE `CompanyHub_alpha`.`loginUsers` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `firstName` VARCHAR(45) NOT NULL,
  `lastname` VARCHAR(45) NULL,
  `phone` VARCHAR(45) NULL,
  `email` VARCHAR(45) NULL,
  `password` VARCHAR(45) NULL,
  `userid` VARCHAR(45) NULL,
  `logingroupid` VARCHAR(5) NULL,
  PRIMARY KEY (`id`));