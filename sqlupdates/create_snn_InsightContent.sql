CREATE TABLE `PlatformID_alpha`.`snn_InsightContent` (
  `id` INT NOT NULL AUTO_INCREMENT,
  `create_time` DATETIME NULL,
  `update_time` DATETIME NULL,
  `type` INT NULL,
  `title` VARCHAR(350) NULL,
  `body` LONGTEXT NULL,
  PRIMARY KEY (`id`));